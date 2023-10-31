import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

// роунд-робин ! - * - * -
/*
const hubUrls = [
    'http://neu-api.tech:7001/notificationHub',
    'http://neu-api.tech:7002/notificationHub',
    'http://neu-api.tech:7003/notificationHub'
];
*/
const hubUrls = [
    'http://localhost:7001/notificationHub',
    'http://localhost:7002/notificationHub',
    'http://localhost:7003/notificationHub'
];

let nextUrlIndex = 0;
// - * - * - * - * - * - * -

function getNextHubUrl() {
    const url = hubUrls[nextUrlIndex];
    nextUrlIndex = (nextUrlIndex + 1) % hubUrls.length;
    console.log('Selected hub URL:', url);
    return url;
}

export const connectToSignalRHub = () => {
    var userId = "0e868f0a-d150-4392-abba-c9b98d4d010a";
    const connection = new HubConnectionBuilder()
        .withUrl('http://localhost:7002/notificationHub', { withCredentials: true })
        //.withUrl(getNextHubUrl(), { withCredentials: true })
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();

    connection.onreconnected(connectionId => {
        console.log('Reconnected to server with connectionId:', connectionId);
    });

    connection.onreconnecting(error => {
        console.log('Reconnecting...', error);
    });

    connection.onclose(error => {
        if (error) {
            console.error('Connection closed with an error:', error);
        } else {
            console.log('Connection closed.');
        }
    });

    connection.on('error', error => {
        console.error('SignalR error:', error);
    });

    connection.on('ReceiveNotification', (message) => {
        console.log('Received notification:', message);
    });

    connection.start()
        .then(() => {
            console.log('Connection started!');
            connection.invoke('JoinGroup', userId)
                .then(() => console.log(`Subscribed to group with userId: ${userId}`))
                .catch(err => console.error('Error while joining group: ', err));
        })
        .catch(err => console.log('Error while establishing connection :(', err));

    return connection;
};
