define(['durandal/system', 'config'],
function (system, config) {

    // Toastr config.
    toastr.options = {
        "debug": false,
        "positionClass": "toast-top-right",
        "onclick": null,
        "fadeIn": 300,
        "fadeOut": 300,
        "timeOut": 3000, // Overridden per message type (info or error - see below)
        "extendedTimeOut": 1000
    }

    return {
        log: log,
        logError: logError,
        logSuccess: logSuccess,
        logInfo: logInfo,
        logWarning: logWarning
    };

    function log(message, data, source, showToast) {
        logIt(message, data, source, showToast, 'info', config.infoDisplayTimeout);
    }

    function logError(message, data, source, showToast) {
        logIt(message, data, source, showToast, 'error', config.errorDisplayTimeout);
    }

    function logSuccess(message, data, source, showToast) {
        logIt(message, data, source, showToast, 'success', config.successDisplayTimeout);
    }

    function logInfo(message, data, source, showToast) {
        logIt(message, data, source, showToast, 'info', config.infoDisplayTimeout);
    }

    function logWarning(message, data, source, showToast) {
        logIt(message, data, source, showToast, 'warning', config.warning, config.warningDisplayTimeout);
    }

    function logIt(message, data, source, showToast, toastType, displayTimeout) {
        source = source ? '[' + source + '] ' : '';

        // Log the console message
        if (data)  system.log(source, message, data);
        else  system.log(source, message);

        // Show the toast
        if (showToast) {
            if(displayTimeout) toastr.options.timeOut = displayTimeout;
            toastr[toastType](message);
        }
    }
});