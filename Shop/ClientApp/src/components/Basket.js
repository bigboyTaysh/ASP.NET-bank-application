import React, { Component } from 'react';
import ProductList from './ProductList';

export default class Basket extends Component {
  
    constructor(props) {
      super(props);
    }
  
    render() {
      return (
        <ProductList data={this.props.data} handleProductRemoveClick={this.props.handleProductRemoveClick} />
      );
    }
  }