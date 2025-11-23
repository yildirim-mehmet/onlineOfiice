// wwwroot/js/excel-realtime.js

(function () {
    if (!window.signalR || !excelModel) {
        console.error("SignalR veya excelModel bulunamadı.");
        return;
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/hub")
        .withAutomaticReconnect()
        .build();

    const sessionId = excelModel.sessionId;
    const sheetName = excelModel.sheetName;
    const userName = excelModel.userName;

    function bindEvents() {
        const cells = document.querySelectorAll(".excel-cell");
        cells.forEach((input) => {
            input.addEventListener("change", (e) => {
                const target = e.target;
                const address = target.getAttribute("data-address");
                const value = target.value;

                connection.invoke("UpdateExcelCell", sessionId, sheetName, address, value)
                    .catch(err => console.error(err.toString()));
            });
        });
    }

    connection.on("CellUpdated", (sheet, address, value) => {
        if (sheet !== sheetName) return;

        const cell = document.querySelector(`.excel-cell[data-address='${address}']`);
        if (cell) {
            cell.value = value ?? "";
            cell.classList.add("bg-warning");
            setTimeout(() => cell.classList.remove("bg-warning"), 500);
        }
    });

    connection.on("UserJoined", (user, count) => {
        console.log(`Kullanıcı katıldı: ${user}. Oturumdaki kişi sayısı: ${count}`);
    });

    connection.start()
        .then(() => {
            console.log("SignalR bağlantısı kuruldu.");
            bindEvents();
            return connection.invoke("JoinExcel", sessionId, userName);
        })
        .catch(err => console.error("SignalR bağlantı hatası:", err));
})();
