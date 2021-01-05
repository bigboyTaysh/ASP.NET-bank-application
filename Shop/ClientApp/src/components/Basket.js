import React, { Component } from 'react';
import BasketForm from './BasketForm';
import ProductList from './ProductList';
import OrderStepper from './OrderStepper';

export class Basket extends Component {

  static displayName = Basket.name;

  render() {
    return (
      <div>
        <ProductList data={this.props.data} handleProductRemoveClick={this.props.handleProductRemoveClick} />
        <BasketForm
          data={this.props.data}
          handleBasketReset={this.props.handleBasketReset}
          handleSetPayment={this.props.handleSetPayment}
          handleSetCardPayment={this.props.handleSetCardPayment}
        />
        <OrderStepper step={0} />
      </div>
    );
  }
}