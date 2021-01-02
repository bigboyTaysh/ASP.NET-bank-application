import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import  Basket  from './components/Basket';
import { FetchData } from './components/FetchData';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import NavBar from './components/NavBar';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  constructor(props) {
    super(props);
    this.state = {
      itemsCount: 0,
      basket: [],
    };
  }

  handleProductAddClick = (product) => {
    this.setState({
      itemsCount: this.state.itemsCount + 1,
      basket: this.state.basket.concat(product),
    });
    console.log(this.state.basket)
  }

  handleProductRemoveClick = (product) => {
    this.setState({
      itemsCount: this.state.itemsCount - 1,
      basket: this.state.basket.filter((p) => p.product !== product),
    });
    console.log(this.state.basket)
  }

  render() {
    return (
      <div>
        <NavBar data={this.state} />
        <Layout>
          <Route exact path='/' render={() => <Home handleProductAddClick={this.handleProductAddClick}/>} />
          <Route exact path='/basket' render={() => <Basket data={this.state} handleProductAddClick={this.handleProductRemoveClick}/>} />
          <AuthorizeRoute path='/fetch-data' component={FetchData} />
          <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
        </Layout>
      </div>
    );
  }
}
