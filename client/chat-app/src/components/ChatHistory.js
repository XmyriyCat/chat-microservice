import React, { useState } from 'react';

const ChatHistory = (props) => {
    const [senderId, setSenderId] = useState('');
    const [receiverId, setReceiverId] = useState('');

    const onSubmit = (e) => {
        e.preventDefault();

        const isUserProvided = senderId && senderId !== '';
        const isSenderProvided = receiverId && receiverId !== '';

        if (isUserProvided && isSenderProvided) {
            props.sendChatHistory(senderId, receiverId);
        }
        else {
            alert('Please insert a user sender id and a user receiver id.');
        }
    }

    const onUserUpdate = (e) => {
        setSenderId(e.target.value);
    }

    const onReceiverUpdate = (e) => {
        setReceiverId(e.target.value);
    }

    return (
        <div>
            <h3>Get chat history</h3>
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

export default ChatHistory;