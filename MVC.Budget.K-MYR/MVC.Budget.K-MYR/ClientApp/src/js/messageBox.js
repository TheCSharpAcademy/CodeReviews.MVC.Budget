class MessageBox {
    #messageBox;
    #messages;
    #index;
    #messagesContainer;
    #icon;
    #isClosing;
    #newMessagesQueue;
    #upButton;
    #downButton;
    #removeInterval;

    constructor() {
        if (MessageBox.instance) {
            return MessageBox.instance;
        }

        this.#messageBox = document.getElementById('messageBox');
        this.#messagesContainer = document.getElementById('messageBox-scrollcontainer');
        this.#icon = document.getElementById('messageBoxIcon');
        this.#upButton = document.getElementById('messageBoxUp-button');
        this.#downButton = document.getElementById('messageBoxDown-button');
        this.#index = 0;
        this.#isClosing = false;
        this.#messages = [];
        this.#newMessagesQueue = [];

        this.#setupEventHandler();
        MessageBox.instance = this;
    }

    #setupEventHandler() {
        var closeButton = document.getElementById('messageBoxClose-button');

        closeButton.addEventListener('dblclick', () => {
            this.hide();
        });

        closeButton.addEventListener('click', () => {
            if (this.#index === 0 && this.#messagesContainer.childElementCount === 1) {
                this.hide();
            } else {
                this.#removeMessage(this.#index);
            }
        });

        this.#messageBox.addEventListener('transitionend', (event) => {           
            if (this.#isClosing && event.propertyName === 'visibility') {
                this.#clearMessages();
                this.#isClosing = false;

                if (this.#newMessagesQueue.length > 0) {
                    this.#newMessagesQueue.forEach(message => this.addMessage(message));
                    this.#newMessagesQueue = [];
                    this.show();
                }
            }
        });


        this.#upButton.addEventListener('click', () => {
            clearInterval(this.#removeInterval);
            this.#move(-1);
        });

        this.#downButton.addEventListener('click', () => {
            clearInterval(this.#removeInterval);
            this.#move(1);
        });
    }

    #move(step) {
        var newIndex = this.#index + step;
        if (newIndex < this.#messagesContainer.childElementCount && newIndex >= 0) {
            this.#index = newIndex;
            this.#scrollTo(this.#index);
            this.#setIcon();
            this.#setButtons();
        }
    }

    #scrollTo(index, behavior = 'smooth') {
        var messageHeight = 48;
        if (index < this.#messagesContainer.childElementCount && index >= 0) {        
            this.#messagesContainer.scrollTo({
                top: index * messageHeight,
                behavior: behavior
            });
        }
    }

    addAndShow(text, iconId, fade = true) {
        this.addMessage({ text: text, iconId: iconId });
        this.show(fade);
    }

    addMessage(messageObject) {
        if (this.#isClosing) {
            this.#newMessagesQueue.push(messageObject);
        } else {
            var messageContainer = document.createElement('div');
            messageContainer.className = 'messageBoxMessage';
            var text = document.createElement('span');
            text.className = 'messageBox-text';
            text.textContent = messageObject.text;
            messageContainer.appendChild(text);
            this.#messagesContainer.appendChild(messageContainer);
            this.#messages.push(messageObject);
            this.#setButtons();
        }
    }

    #removeMessage(index = 0) { 
        var childToRemove = this.#messagesContainer.children[index];
        if (childToRemove) {
            if (index < this.#index) {
                this.#index -= 1;
                this.#scrollTo(this.#index, 'instant');
            }
            this.#messagesContainer.removeChild(childToRemove);            
        }
        this.#messages.splice(index, 1);
        this.#index = Math.min(this.#index, this.#messagesContainer.childElementCount - 1);
        this.#setIcon();
        this.#setButtons();
    }

    show(fade = true) {
        if (!this.#messageBox.classList.contains('show') && !this.#isClosing) {
            if (fade && !this.#removeInterval) {
                this.#removeInterval = setInterval(() => {
                    if (this.#messagesContainer.childElementCount > 1) {
                        this.#removeMessage()
                    } else {
                        this.hide();
                    }
                }, 3000);
            }
            this.#setIcon(this.#index);
            this.#setButtons();
            this.#messageBox.classList.add('show');
        }        
    }

    hide() {
        if (this.#messageBox.classList.contains('show')) {            
            this.#isClosing = true; 
            this.#messageBox.classList.remove('show');
            if (this.#removeInterval) {
                clearInterval(this.#removeInterval);
                this.#removeInterval = null;
            }
        }
    }

    #clearMessages() {
        this.#messagesContainer.replaceChildren(); 
        this.#messages = [];            
    }

    #setIcon() {
        var messageObject = this.#messages[this.#index];
        if (messageObject) {
            this.#icon.setAttribute('href', messageObject.iconId);
        }
    }

    #setButtons() {
        this.#index > 0 ? this.#upButton.disabled = false : this.#upButton.disabled = true;            
        this.#index < this.#messages.length - 1 ? this.#downButton.disabled = false : this.#downButton.disabled = true;
    }
}

export default new MessageBox();
