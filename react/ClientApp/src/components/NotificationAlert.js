import React, { useState, useEffect } from 'react';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import 'react-notifications/lib/notifications.css';

const NotificationAlert = ({ connection }) => {
    useEffect(() => {
        connection.on('ReceiveNotification', (message) => {
            NotificationManager.info(message);
        });
    }, [connection]);

    return (
        <div>
            <NotificationContainer />
        </div>
    );
};

export default NotificationAlert;
