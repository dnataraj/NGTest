import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import { NavMenu } from './NavMenu';
import { Home } from './Home';

export class Layout extends Component {
  constructor(props) {
    super(props);
    this.state = {
      connection: null,
      message: '',
      conversation: [],
      connectedUsers: [],
      handle: null
    }

    this.updateConnectedUsers = this.updateConnectedUsers.bind(this);
  }

  displayName = Layout.name

  updateConnectedUsers(users) {
    console.log("updating connected users...");
    console.dir(users);
    this.setState({connectedUsers: users});
  }

  render() {
    return (
      <Grid fluid>
        <Row>
          <Col sm={3}>
            <NavMenu users={this.state.connectedUsers} />
          </Col>
          <Col sm={9}>
            <Home showUsers={ users => this.updateConnectedUsers(users)} />
          </Col>
        </Row>
      </Grid>
    );
  }
}
