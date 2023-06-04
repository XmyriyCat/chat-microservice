import React, { useState, useEffect, useRef } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

import ChatWindow from './ChatWindow';
import ChatInput from './ChatInput';
import ChatHistory from './ChatHistory';

const Chat = () => {
    const [connection, setConnection] = useState(null);
    const [chat, setChat] = useState([]);
    const latestChat = useRef(null);

    latestChat.current = chat;

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5000/hubs/chat')
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');

                    connection.on('ReceiveFullMessage', message => {
                        const updatedChat = [...latestChat.current];
                        updatedChat.push(message);

                        setChat(updatedChat);
                    });

                    connection.on('ReceiveChatHistory', messages => {
                        const updatedChat = [...latestChat.current];
                        updatedChat.push.apply(updatedChat, messages);

                        setChat(updatedChat);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    const sendMessage = async (userSenderId, message, userReceiverId) => {
        const createMessageDto = {
            senderUserId: Number(userSenderId),
            value: message,
            receiverUserId: Number(userReceiverId)
        };

        try {
            await connection.send('SendFullMessage', createMessageDto);
        }
        catch (e) {
            console.log(e);
        }
    }

    const sendChatHistory = async (userSenderId, userReceiverId) => {
        const getMessagesRequestDto = {
            senderUserId: Number(userSenderId),
            receiverUserId: Number(userReceiverId)
        };

        try {
            await connection.send('SendChatHistoryRequest', getMessagesRequestDto);
        }
        catch (e) {
            console.log(e);
        }
    }

    return (
        <div>
            <table>
                <tbody>
                    <tr>
                        <td><ChatInput sendMessage={sendMessage} /></td>
                        <td></td>
                        <td><ChatHistory sendChatHistory={sendChatHistory} /></td>
                    </tr>
                </tbody>
            </table>
            <hr />
            <ChatWindow chat={chat} />
        </div>
    );
};

export default Chat;