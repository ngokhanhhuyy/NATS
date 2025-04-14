class AjaxDeleteController {
    /**
     * @param {HTMLButtonElement} showModalButtonElement
     */
    constructor(showModalButtonElement) {
        let confirmationModalController = new DeleteConfirmationModalController();

        showModalButtonElement.addEventListener("click", () => {
            confirmationModalController.showModal();
        })
    }
}

class DeleteConfirmationModalController {
    constructor() {
        /**
         * @type {HTMLElement}
         */
        let modalElement = document.getElementById("delete-confirmation-modal");
        this.modal = new bootstrap.Modal(modalElement);
    
        /**
         * @type {HTMLFormElement}
         */
        let formElement = document.getElementById("deleting-form");

        /**
         * @type {HTMLInputElement | HTMLButtonElement}
         */
        let modalOkButtonElement = document.getElementById("delete-confirmation-modal-ok-button");
        modalOkButtonElement.addEventListener("click", () => {
            this.onDeleteConfirmationModalOkButtonClicked(this.modal, formElement, modalOkButtonElement);
        });

        /**
         * @type {HTMLButtonElement}
         */
        let modalCloseButtonElement = document.getElementById("delete-confirmation-modal-cancel-button");
        modalCloseButtonElement.onclick = () => {
            this.modal.hide();
        };
    }

    /**
     * @param {bootstrap.Modal} modal
     * @param {HTMLFormElement} formElement
     * @param {HTMLButtonElement} okButtonElement
     */
    onDeleteConfirmationModalOkButtonClicked(
            modal,
            formElement,
            okButtonElement) {
        /**
         * @type {string}
         */
        let okButtonElementOldInnerHTML = okButtonElement.innerHTML;
        okButtonElement.disabled = true;
        okButtonElement.innerHTML = `
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
        `;

        /**
         * @type {FormData}
         */
        let formData = new FormData(formElement);
        fetch(formElement.action, {
            method: "POST",
            body: formData
        }).then(response => {
            modal.hide();
            if (response.ok) {
                let deleteSuccessModalController = new DeleteSuccessModalController();
                deleteSuccessModalController.showModal();
            } else {
                showOperationErrorNotificationModal();
            }
            okButtonElement.disabled = false;
            okButtonElement.innerHTML = okButtonElementOldInnerHTML;
        }).catch(error => {
            if (error instanceof TypeError) {
                modal.hide();
                okButtonElement.disabled = false;
                okButtonElement.innerHTML = okButtonElementOldInnerHTML;
                showConnectionErrorNotificationModal();
            }
        });
    }

    showModal() {
        this.modal.show();
    }

    hideModal() {
        this.modal.hide();
    }
}

class DeleteSuccessModalController {
    constructor() {
        /**
         * @type {HTMLElement}
         */
        let modalElement = document.getElementById("delete-success-notification-modal");
        this.modal = new bootstrap.Modal(modalElement);
    }

    showModal() {
        this.modal.show();
    }

    hideModal() {
        this.modal.hide();
    }
}