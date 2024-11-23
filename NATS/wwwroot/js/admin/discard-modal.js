class DiscardModalController {
    /**
     * @param {HTMLButtonElement} showModalButtonElement
     */
    constructor(showModalButtonElement) {
        /**
         * @type {HTMLElement}
         */
        let modalElement = document.getElementById("discard-confirmation-modal");
    
        /**
         * @type {HTMLButtonElement}
         */
        let modalCloseButtonElement = document.getElementById("discard-confirmation-modal-cancel-button");
        let modal = new bootstrap.Modal(modalElement);
        modalCloseButtonElement.onclick = () => {
            modal.hide();
        };

        showModalButtonElement.addEventListener("click", () => {
            modal.show();
        })
    }
}