
function showMessageBox(receivedUserId) {
    var xhttp = new XMLHttpRequest();
    //ссылка по которой переходим в екшн-метод в контоллере + ID пользователя
    var href = "/Message/SendFirstMessage?receivedUserId=" + encodeURIComponent(receivedUserId);

    //описываем параметры запроса: (тип, ссылка, асинхронность)
    xhttp.open("GET", href, false);
    xhttp.send();

    //проверяем все ли хорошо, если да, вставляем в "MessageArea" наш ответ
    if (xhttp.readyState == 4 && xhttp.status == 200) {
        document.getElementById("MessageArea").innerHTML = xhttp.responseText;
    }

    //не хочет отправлять не асинхронно, по-этому не подходит
    //$.get(href, function (data, status) {
    //    if (status == "success")
    //        document.getElementById("MessageArea").innerHTML = data;
    //    if (status == "error")
    //        alert("Error: " + status);
    //});

    //модально открываем окно для ввода сообщения
    $("#myModal").modal();
}

function MessageOnSuccess(result) {
    alert("OLOLOOLOLO");
}

function MessageOnFailure(result) {
    alert("Fuckup");
}