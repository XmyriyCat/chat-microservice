import React, { useState } from 'react';

const ChatInput = (props) => {
    const [senderId, setSenderId] = useState('');
    const [message, setMessage] = useState('');
    const [receiverId, setReceiverId] = useState('');

    const onSubmit = (e) => {
        e.preventDefault();

        const isUserProvided = senderId && senderId !== '';
        const isMessageProvided = message && message !== '';
        const isSenderProvided = receiverId && receiverId !== '';

        if (isUserProvided && isMessageProvided && isSenderProvided) {
            props.sendMessage(senderId, message, receiverId);
        }
        else {
            alert('Please insert a user sender, a message and a user receiver.');
        }
    }

    const onUserUpdate = (e) => {
        setSenderId(e.target.value);
    }

    const onMessageUpdate = (e) => {
        setMessage(e.target.value);
    }

    const onReceiverUpdate = (e) => {
        setReceiverId(e.target.value);
    }

    return (
        <div>
            <h3>Send message</h3>
            <form
                onSubmit={onSubmit}>
                <label htmlFor="user">User sender ID:</label>
                <br />
                <input
                    type="number"
                    id="user"
                    name="user"
                    value={senderId}
                    onChange={onUserUpdate} />
                <br />
                <label htmlFor="message">Message:</label>
                <br />
                <input
                    type="text"
                    id="message"
                    name="message"
                    value={message}
                    onChange={onMessageUpdate} />
                <br />
                <label htmlFor="user">User receiver ID:</label>
                <br />
                <input
                    type="number"
                    id="userReceiver"
                    name="userReceiver"
                    value={receiverId}
                    onChange={onReceiverUpdate} />
                <br /><br />
                <button>Submit</button>
            </form>
        </div>
    )
};

export default ChatInput;