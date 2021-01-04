import React, { Component } from 'react';
import BasketForm from './BasketForm';
import ProductList from './ProductList';

export class Basket extends Component {

  static displayName = Basket.name;

  render() {
    return (
      <div>
        <ProductList data={this.props.data} handleProductRemoveClick={this.props.handleProductRemoveClick} />
        <BasketForm data={this.props.data} />
      </div>
    );
  }
}