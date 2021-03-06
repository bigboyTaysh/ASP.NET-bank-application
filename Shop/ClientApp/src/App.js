import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Basket } from './components/Basket';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import NavBar from './components/NavBar';
import Summary from './components/Summary';
import PaymentForm from './components/PaymentForm';

import './custom.css'
import authService from './components/api-authorization/AuthorizeService';

export default class App extends Component {
  static displayName = App.name;

  constructor(props) {
    super(props);

    this.state = {
      itemsCount: 0,
      basketPrice: 0,
      basket: [],
      payment: false,
      cardPayment: false,
    };
  }

  componentDidMount = () => {
    const data = JSON.parse(localStorage.getItem("state"));
    if (data) {
      this.setState({
        itemsCount: data.itemsCount,
        basketPrice: (parseFloat(data.basketPrice)).toFixed(2),
        basket: data.basket,
        cardPayment: data.cardPayment
      });
    }
  }

  handleProductAddClick = (product) => {
    this.setState({
      itemsCount: this.state.itemsCount + 1,
      basketPrice: (parseFloat(this.state.basketPrice) + parseFloat(product.salePrice)).toFixed(2),
      basket: this.state.basket.concat(product),
    }, function () {
      localStorage.setItem("state", JSON.stringify({
        itemsCount: this.state.itemsCount,
        basketPrice: this.state.basketPrice,
        basket: this.state.basket,
        cardPayment: this.state.cardPayment
      }))
    });
  }

  handleProductRemoveClick = (index) => {
    var basket = this.state.basket;
    var price = basket[index].salePrice;
    basket.splice(index, 1);

    this.setState({
      itemsCount: this.state.itemsCount - 1,
      basketPrice: (parseFloat(this.state.basketPrice) - parseFloat(price)).toFixed(2),
      basket: basket,
    }, function () {
      localStorage.setItem("state", JSON.stringify({
        itemsCount: this.state.itemsCount,
        basketPrice: this.state.basketPrice,
        basket: this.state.basket,
        cardPayment: this.state.cardPayment
      }))
    });
  }

  handleBasketReset = () => {
    this.setState({
      itemsCount: parseFloat(0),
      basketPrice: parseFloat(0),
      basket: [],
    }, function () {
      localStorage.clear()
    });
  }

  handleSetPayment = (state) => {
    this.setState({
      payment: state
    });
  }

  handleSetCardPayment = (state) => {
    this.setState({
      cardPayment: state
    });
  }

  render() {
    return (
      <div>
        <NavBar data={this.state} />
        <Layout>
          <Route exact path='/' render={() => <Home handleProductAddClick={this.handleProductAddClick} />} />
          <Route exact path='/basket'
            render={() =>
              <Basket
                data={this.state}
                handleProductRemoveClick={this.handleProductRemoveClick}
                handleBasketReset={this.handleBasketReset}
                handleSetPayment={this.handleSetPayment}
                handleSetCardPayment={this.handleSetCardPayment}
              />
            }
          />
          <Route exact path='/payment'
            render={(props) =>
              <PaymentForm
                {...props}
                data={this.state}
                handleBasketReset={this.handleBasketReset}
                handleSetCardPayment={this.handleSetCardPayment}
              />}
          />
          <Route path='/summary/:id'
            render={(props) =>
              <Summary
                {...props}
                data={this.state}
                handleBasketReset={this.handleBasketReset}
                handleSetPayment={this.handleSetPayment}
              />}
          />
          <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
        </Layout>
      </div>
    );
  }
}
