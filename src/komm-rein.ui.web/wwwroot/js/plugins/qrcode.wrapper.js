export function showQrCode(qrData) {
    var output_div = document.getElementById(qrData.id)

    var qrcode = new QRCode(output_div, {
        width: qrData.width,
        height: qrData.height,
        colorDark: qrData.colorDark,
        colorLight: qrData.colorLight,
        correctLevel: QRCode.CorrectLevel.H
    });

    qrcode.makeCode(qrData.value);
}