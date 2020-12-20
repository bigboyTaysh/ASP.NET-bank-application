import { AppBar } from '@material-ui/core';
import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import NavBar from './NavBar';

export class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
      <div>
        <NavBar />
        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }
}
