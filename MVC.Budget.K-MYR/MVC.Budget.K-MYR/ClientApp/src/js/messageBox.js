export default class MessageBox {
    #messageBox;
    #messages;
    #index;
    #container;
    #icon;
    #isClosing;
    #newMessagesQueue;

    constructor() {
        this.#messageBox = document.getElementById('messageBox');
        this.#container = document.getElementById('messageBox-scrollcontainer');
        this.#icon = document.getElementById('messageBoxIcon');
        this.#index = 0;
        this.#isClosing = false;
        this.#messages = [];
        this.#newMessagesQueue = [];

        var closeButton = document.getElementById('messageBoxClose-button');

        closeButton.addEventListener('dblclick', () => {
            this.#hide();
        });

        closeButton.addEventListener('click', () => {
            if (this.#index === 0 && this.#container.childElementCount === 1) {
                this.#hide();
            } else {
                this.#removeMessage();
            }
        });

        this.#messageBox.addEventListener('transitionend', () => {
            if (this.#isClosing) {
                this.#clearMessages();
                this.#isClosing = false;

                if (this.#newMessagesQueue.length > 0) {
                    this.#newMessagesQueue.forEach(message => this.addMessage(message));
                    this.#newMessagesQueue = [];
                }
            }
        });
    }

    addMessage(messageObject) {
        if (this.#isClosing) {
            this.#newMessagesQueue.push(messageObject);
        } else {
            var messageContainer = document.createElement('div');
            messageContainer.className = 'messageBoxMessage';
            var text = document.createElement('span');
            text.textContent = messageObject.text;
            messageContainer.appendChild(text);
            this.#container.appendChild(messageContainer);
            this.#messages.push(messageObject);
            this.#show();
        }
    }

    #removeMessage() { 
        var childToRemove = this.#container.children[this.#index];
        if (childToRemove) {            
            this.#container.removeChild(childToRemove);
            this.#messages.splice(this.#index, 1);
            if (this.#index > this.#container.childElementCount) {
                index = this.#container.childElementCount;
            }
            this.#setIcon();
        }              
    }

    #show() {
        this.#setIcon(this.#index);
        this.#messageBox.classList.add('show');
    }

    #hide() {
        this.#isClosing = true; 
        this.#messageBox.classList.remove('show');
    }

    #clearMessages() {
        this.#container.replaceChildren(); 
        this.#messages = [];            
    }

    #setIcon() {
        var messageObject = this.#messages[this.#index];
        if (messageObject) {
            this.#icon.setAttribute('href', messageObject.iconId);
        }
    }
}
