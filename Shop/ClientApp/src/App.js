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
    }
  }

  componentDidMount = () => {
    const data = JSON.parse(localStorage.getItem("state"));
    if(data){
      this.setState({
        itemsCount: data.itemsCount,
        basket: data.basket
      });
    }
  }

  handleProductAddClick = (product) => {
    this.setState({
      itemsCount: this.state.itemsCount + 1,
      basket: this.state.basket.concat(product),
    }, function () {
      localStorage.setItem("state", JSON.stringify(this.state))
    });
  }

  handleProductRemoveClick = (index) => {
    var basket = this.state.basket;
    basket.splice(index, 1);

    this.setState({
      itemsCount: this.state.itemsCount - 1,
      basket: basket, 
    }, function () {
      localStorage.setItem("state", JSON.stringify(this.state))
    });
  }

  render() {
    return (
      <div>
        <NavBar data={this.state} />
        <Layout>
          <Route exact path='/' render={() => <Home handleProductAddClick={this.handleProductAddClick}/>} />
          <Route exact path='/basket' render={() => <Basket data={this.state} handleProductRemoveClick={this.handleProductRemoveClick}/>} />
          <AuthorizeRoute path='/fetch-data' component={FetchData} />
          <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
        </Layout>
      </div>
    );
  }
}
