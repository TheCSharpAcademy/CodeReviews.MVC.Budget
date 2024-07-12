export class Modal {
    static async showPartialViewFormInModal(uri, modalTitle) {
        const modalViewResult = await fetch(uri);
        const modalContent = await modalViewResult.text();

        const { modalContentEl } = Modal.setModalContent(modalTitle, modalContent);

        if (!modalContentEl) return;

        const forms = modalContentEl?.getElementsByTagName("form");
        var newForm = forms?.[forms.length - 1];
        if (newForm) {
            $.validator.unobtrusive.parse(newForm);
        }
    }

    static showConfirmationModal({ title, body, action, onConfirm, isDestructive, cancelable }) {
        if (!document.getElementById("confirmModalContainer")) {
            document.body.insertAdjacentHTML("beforeend", `<div id="confirmModalContainer"></div>`);
        }

        const container = document.getElementById("confirmModalContainer");
        container.innerHTML = '';

        const modalHtml = `
        <div class="modal fade" id="confirmModal" tabindex="-1" aria-labelledby="confirmModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="confirmModalLabel">${title}</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    ${body}
                </div>
                <div class="modal-footer">
                    ${cancelable ? '<button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>' : ''}
                    <button type="button" class="btn ${isDestructive ? 'btn-danger' : ''}" id="confirmModalAction">${action}</button>
                </div>
                </div>
            </div>
          </div>
    `;

        container.innerHTML = modalHtml;

        const confirmButton = document.getElementById('confirmModalAction');
        const modalEl = document.getElementById('confirmModal');

        if (!confirmButton || !modalEl) {
            return;
        }

        const confirmModal = new bootstrap.Modal(modalEl);

        confirmModal.show();

        confirmButton.addEventListener('click', async function () {
            try {
                await onConfirm();
                confirmModal?.hide();
            } catch {
                confirmModal?.hide();
                showFailureModal();
            } finally {
                confirmModal?.hide();
            }
        });

    }

    static showFailureModal() {
        Modal.showConfirmationModal({ title: "Error", body: "Operation failed", action: "OK", onConfirm: () => { }, cancelable: false });
    }

    static setModalContent(title = "", content = "") {
        const modalEl = document.getElementById("modal");
        const modalTitleEl = modalEl?.querySelector(".modal-title");
        const modalContentEl = modalEl?.querySelector(".modal-body");

        if (!modalEl || !modalContentEl || !modalTitleEl) {
            console.log("could not set modal content");
            return {};
        }

        modalTitleEl.innerHTML = title;
        modalContentEl.innerHTML = content;

        return { modalTitleEl, modalContentEl };
    }
}