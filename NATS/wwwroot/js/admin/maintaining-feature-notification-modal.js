function showMaintainingFeatureNotificationModal() {
    /**
     * @type {HTMLElement}
     */
    let modalElement = document.getElementById("maintaining-feature-notification-modal");

    /**
     * @type {HTMLButtonElement}
     */
    let modalCloseButtonElement = document.getElementById("maintaining-feature-notification-modal-cancel-button");
    let modal = new bootstrap.Modal(modalElement);
    modalCloseButtonElement.onclick = () => {
        modal.hide();
    };
    modal.show();
}