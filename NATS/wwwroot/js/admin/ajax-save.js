class AjaxSaveController {
    /**
     * @param {HTMLFormElement} formElement
     * @param {string} redirectingUrl
     * @param {boolean} editableAfterSave
     */
    constructor(formElement, redirectingUrl, editableAfterSave) {
        this.formElement = formElement;
        let successModalController = new SaveSuccessModalController(redirectingUrl, editableAfterSave);
        let validationErrorModalController = new ValidationErrorModalController();
        let operationErrorModalController = new OperationErrorModalController();
        let connectionErrorModalController = new SaveConnectionErrorModalController();

        /**
         * @type {HTMLButtonElement | HTMLInputElement}
         */
        let submitButtonElement = document.getElementById(formElement.id + "-submit-button");

        let submitButtonLoadingInnerHTML = `
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
        `;
        let submitButtonOldInnerHTML;
        if (submitButtonElement) {
            submitButtonOldInnerHTML = submitButtonElement.innerHTML;
        }

        formElement.addEventListener("submit", (event) => {
            event.preventDefault();

            // Disable submit button and display as loading
            submitButtonElement.disabled = true;
            submitButtonElement.innerHTML = submitButtonLoadingInnerHTML;

            /**
             * @type {FormData}
             */
            let formData = new FormData(formElement);
            fetch(formElement.action, {
                method: "post",
                body: formData
            }).then(response => {
                submitButtonElement.innerHTML = submitButtonOldInnerHTML;
                submitButtonElement.disabled = false;
                // Saved successfully with 200 Ok response
                if (response.ok) {
                    successModalController.showModal();
                    this.configureValidationAppearance();
                // Saved failed with 400 Bad Request response
                } else if (response.status === 400) {
                    // Server responded with validation error information
                    response.json().then(responseDto => {
                        validationErrorModalController.showModal();
                        this.configureValidationAppearance(responseDto);
                    // Server responeded with operation error information,
                    // due to client-side bug or someone has tweaked the client-side logic.
                    }).catch(error => {
                        if (error instanceof SyntaxError) {
                            operationErrorModalController.showModal();
                        }
                    });
                // Saved failed with 404 Not Found response (client-side logic has been tweaked)
                // or 500 Server Error response (server-side bug)
                } else  {
                    operationErrorModalController.showModal();
                }
            }).catch(error => {
                if (error instanceof TypeError) {
                    submitButtonElement.innerHTML = submitButtonOldInnerHTML;
                    submitButtonElement.disabled = false;
                    connectionErrorModalController.showModal();
                }
            });
        });
    }

    /**
     * @param {Object}
     */
    configureValidationAppearance(errorResponseDto) {
        /**
         * @type {HTMLInputElement[] | HTMLTextAreaElement[] | HTMLSelectElement[]}
         * Retriving all input elements associated to the form
         */
        let inputElements = document.querySelectorAll(
            `input[form="${this.formElement.id}"]:not([type="checkbox"]):not([type="radio"]), ` +
            `textarea[form="${this.formElement.id}"], ` +
            `select[form="${this.formElement.id}"]`);
        Array.from(inputElements).forEach(inputElement => {
            /**
             * @type {HTMLSpanElement}
             */
            let messageElement = document.querySelector(`span[data-valmsg-for="${inputElement.name}"]`);

            // Invalid when response dto contains input name attribute value
            if (errorResponseDto && inputElement.name in errorResponseDto) {
                inputElement.classList.remove("is-valid");
                inputElement.classList.add("is-invalid");
                if (messageElement) {
                    /**
                     * @type {string[]}
                     */
                    let messageContents = errorResponseDto[inputElement.name];
                    messageElement.textContent = messageContents[0];
                    messageElement.classList.add("d-inline");
                    messageElement.classList.remove("valid-feedback");
                    messageElement.classList.add("invalid-feedback");
                }
            }
            // Valid
            else {
                inputElement.classList.remove("is-invalid");
                inputElement.classList.add("is-valid");
                if (messageElement) {
                    messageElement.textContent = "";
                    messageElement.classList.add("d-inline");
                    messageElement.classList.remove("invalid-feedback");
                    messageElement.classList.add("valid-feedback");
                }
            }
        });
    }
}

class SaveSuccessModalController {
    /**
     * @param {string} redirectUrl
     * @param {boolean} editableAfterSave
     */
    constructor(redirectUrl, editableAfterSave) {
        this.redirectUrl = redirectUrl;
        this.editableAfterSave = editableAfterSave;
        /**
         * @type {HTMLElement}
         */
        let modalElement = document.getElementById("save-success-modal");
        this.modal = new bootstrap.Modal(modalElement);
    
        /**
         * @type {HTMLButtonElement | null}
         */
        let okButtonElement = document.getElementById("save-success-modal-ok-button");
        if (okButtonElement) {
            okButtonElement.onclick = () => {
                this.modal.hide();
            };
        }
    }

    showModal() {
        this.modal.show();
    }

    hideModal() {
        this.modal.hide();
    }
}

class ValidationErrorModalController {
    constructor() {
        /**
         * @type {HTMLElement}
         */
        let modalElement = document.getElementById("validation-error-modal");

        /**
         * @type {HTMLButtonElement}
         */
        let okButtonElement = document.getElementById("validation-error-modal-ok-button");
        this.modal = new bootstrap.Modal(modalElement);
        okButtonElement.onclick = () => {
            this.modal.hide();
        };
    }

    showModal() {
        this.modal.show();
    }

    hideModal() {
        this.modal.hide();
    }
}

class OperationErrorModalController {
    constructor() {
        /**
         * @type {HTMLElement}
         */
        let modalElement = document.getElementById("operation-error-modal");
    
        /**
         * @type {HTMLButtonElement}
         */
        let okButtonElement = document.getElementById("operation-error-modal-ok-button");
        this.modal = new bootstrap.Modal(modalElement);
        okButtonElement.onclick = () => {
            this.modal.hide();
        };
    }

    showModal() {
        this.modal.show();
    }

    hideModal() {
        this.modal.hide();
    }
}

class SaveConnectionErrorModalController {
    constructor() {
        /**
         * @type {HTMLElement}
         */
        let modalElement = document.getElementById("save-connection-error-modal");

        /**
         * @type {HTMLButtonElement}
         */
        let okButtonElement = document.getElementById("save-connection-error-modal-ok-button");
        this.modal = new bootstrap.Modal(modalElement);
        okButtonElement.onclick = () => {
            this.modal.hide();
        };
    }

    showModal() {
        this.modal.show();
    }

    hideModal() {
        this.modal.hide();
    }
}