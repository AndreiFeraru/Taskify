window.openModal = function (modalId) {
    var modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.add('show');
        modal.style.display = 'block';
        document.body.classList.add('modal-open');
    }
};

window.closeModal = function (modalId) {
    var modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.remove('show');
        modal.style.display = 'none';
        document.body.classList.remove('modal-open');
    }
};

window.showError = function (errorText) {
    if (errorText && errorText != "")
        alert(errorText);
};