const { AvailableSensorsRequest, AvailableSensorsResponse, TemperatureRequest } = require('./SensorService_pb.js');
const { SensorServiceClient } = require('./SensorService_grpc_web_pb.js');


var client = new SensorServiceClient("https://localhost:5001");

var usr = document.getElementById('txt_username');
var msg = document.getElementById('txt_message');
var deviceid = document.getElementById('txt_deviceid');

var sendInput = document.getElementById('send');
var streamInput = document.getElementById('stream');
var resultText = document.getElementById('result');
var streamingCall = null;

// Unary call
sendInput.onclick = function () {
    var request = new AvailableSensorsRequest();
    request.setUsername(usr.value);
    request.setMessage(msg.value);

    client.getAvailableSensors(request, {}, (err, response) => {
        resultText.innerHTML = "Message from server: " + htmlEscape(response.getMessage()) + " Devices: " + htmlEscape(response.getDevices());
    });
};

// Server streaming call
streamInput.onclick = function () {
    if (!streamingCall) {
        sendInput.disabled = true;
        streamInput.value = 'Stop server stream';
        resultText.innerHTML = '';

        var request = new TemperatureRequest();
        request.setDeviceid(deviceid.value);

        streamingCall = client.receiveTemperatureUpdates(request, {});
        streamingCall.on('data', function (response) {
            resultText.innerHTML += "Device location: " + htmlEscape(response.getDevicelocation()) + " Temperature: " + htmlEscape(response.getTemperature()) + '<br />';
        });
        streamingCall.on('end', function () {
        });
    } else {
        streamingCall.cancel();
        streamingCall = null;
        sendInput.disabled = false;
        streamInput.value = 'Start server stream';
    }
};

function htmlEscape(str) {
    return String(str)
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}