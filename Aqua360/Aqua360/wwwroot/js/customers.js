window.aqua360 = window.aqua360 || {};
window.aqua360.confirmCustomerDelete = function (customerName) {
    return window.confirm(`¿Eliminar al cliente ${customerName}? Esta acción no se puede deshacer.`);
};
