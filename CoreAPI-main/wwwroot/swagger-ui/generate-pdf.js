window.addEventListener("load", function () {
    customizeSwaggerUI();
});
function customizeSwaggerUI() {
    setTimeout(function () {
        var tag = '<rapi-pdf style="display:none" id="thedoc" pdf-schema-style="table" pdf-footer-text="JITS - Just In Times Solution"> </rapi-pdf>';
        var btn = '<button id="btn" style="font-size:16px;padding: 6px 16px;text-align: center;white-space: nowrap;border-radius: 3px; background-color:#61affe;color: white;border: 0px solid #333;cursor: pointer;" type="button" onclick="downloadPDF()">Download Document</button>';
        var oldhtml = document.getElementsByClassName('info')[0].innerHTML;
        document.getElementsByClassName('info')[0].innerHTML = oldhtml + tag + btn;
    }, 1200);
}
function downloadPDF() {
    var client = new XMLHttpRequest();
    client.overrideMimeType("application/json");
    client.open('GET', '/CoreAPI/V1.0a/CoreAPI.json');
    var jsonAPI = "";
    client.onreadystatechange = function () {
        if (client.responseText != 'undefined' && client.responseText != "") {
            jsonAPI = client.responseText;
            if (jsonAPI != "") {
                let docEl = document.getElementById("thedoc");
                var key = jsonAPI.replace('\"Authorization: Bearer {token}\"', "");
                let objSpec = JSON.parse(key);
                docEl.generatePdf(objSpec);
            }
        }
    }
    client.send();
}