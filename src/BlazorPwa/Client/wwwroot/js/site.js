let handler;

window.Connection = {
    Initialize: function (interop) {

        const updateOnlineStatus = function () {
            console.log("Parte aggiornamento stato", navigator.onLine);
            interop.invokeMethodAsync("OnConnectionStatusChanged", navigator.onLine);
        }

        window.addEventListener("online", updateOnlineStatus);
        window.addEventListener("offline", updateOnlineStatus);
        updateOnlineStatus();
    },
    Dispose: function () {

        if (updateOnlineStatus != null) {

            window.removeEventListener("online", updateOnlineStatus);
            window.removeEventListener("offline", updateOnlineStatus);
        }
    }
};