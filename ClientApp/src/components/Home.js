import React, { Component } from 'react';
import { InputGroup } from 'react-bootstrap';

import { HubConnectionBuilder } from '@aspnet/signalr';

export class Home extends Component {
  constructor(props) {
    super(props);
    this.state = {
      connection: null,
      message: '',
      conversation: [],
      connectedUsers: [],
      handle: null
    }

    this.setHandle = this.setHandle.bind(this);
    this.sendMessage = this.sendMessage.bind(this);
    this.updateConversations = this.updateConversations.bind(this);
  }

  componentDidMount() {
    let connection = new HubConnectionBuilder().withUrl("/chat").build();

    // register handlers, and start
    connection.on("ReceiveBroadcast", (user, message, timestamp) => {
      const recvd = `[${timestamp}] ${user} --> ${message}`;
      this.updateConversations(recvd);
    });
    connection.on("UpdateConnectedUsers", (users) => this.updateConnectedUsers(users));

    connection.start().then(() => console.log("Ready to go...")).catch(err => console.error(err.toString()));

    this.setState({connection});
  }

  setHandle(event) {
    event.preventDefault();
    const data = new FormData(event.target);
    const userHandle = data.get("handle");

    // call connect on hub
    this.state.connection.invoke('Connect', userHandle).catch(err => console.error(err.toString()));

    this.setState({handle: userHandle}, () => {
      // update list of connected uses
      this.state.connection.invoke('BroadcastConnectedUsers')
      //.then(users => this.props.showUsers(users))
      .catch(err => console.error(err.toString()));
    });
  }

  updateConversations(text) {
    const chat = this.state.conversation.slice()
    chat.push(text);
    this.setState({conversation: chat});    
  }

  updateConnectedUsers(users) {
    this.props.showUsers(users);
  }

  sendMessage(event) {
    event.preventDefault();
    this.state.connection.invoke('BroadcastMessage', this.state.handle, this.state.message, new Date().toUTCString()).catch(err => console.error(err.toString()));    
    //this.updateConversations(message);

    this.setState({message: ''});

  }

  displayName = Home.name

  render() {
    return (
      <div>
        {
          this.state.handle === null ?
          <React.Fragment>
            <h1>Hello, and welcome to NG Chat!</h1>
            <p>Please enter your handle :</p>
            <br />
            <form onSubmit={this.setHandle}>
              <input type="text" name="handle" />
              <input type="submit" value="Connect" />
            </form>
          </React.Fragment>
          :
          <React.Fragment>
            <h1>Hello {this.state.handle}!</h1>
            <div>
              <p>What would you like to say?</p>
              <form onSubmit={this.sendMessage}>
                <input type="text" name="message" value={this.state.message} onChange={event => this.setState({ message: event.target.value })} />
                <input type="submit" value="Send" />
              </form>
            </div>
          </React.Fragment>  
        }
        {
          this.state.conversation.length > 0 ?
          <React.Fragment>
            <h2>The conversation so far...</h2>
            <ul>
              {
                this.state.conversation.map( (text, index) => (<li key={index}>{text}</li>))
              }
            </ul>
          </React.Fragment>      
          :
          null           
        }
      </div>
    );
  }
}
