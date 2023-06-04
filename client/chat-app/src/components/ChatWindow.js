import React from 'react';

import Message from './Message';

const ChatWindow = (props) => {
    const chat = props.chat
        .map(m => <Message 
            key={Date.now() * Math.random()}
            userSender={m.senderUsername}
            message={m.value}
            userReceiver={m.receiverUsername}/>);

    return(
        <div>
            {chat}
        </div>
    )
};

export default ChatWindow;