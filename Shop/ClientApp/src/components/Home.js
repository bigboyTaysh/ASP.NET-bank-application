import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService';
import Categories from './Categories';
import { CSSTransition } from 'react-transition-group';
import ProductCars from './ProductCars';


export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = {
      categories: [],
      products: [],
      categoriesLoading: true,
      productsLoading: true,
      skeleton: [0, 1, 2],
    };
  }

  componentDidMount() {
    this.populateCategories();
    this.populateProducts(0);
  }

  handleCategoriesClick = (value) => {
    this.setState({ productsLoading: true });
    this.populateProducts(value);
  }

  handleProductAddClick = (product) => {
    this.props.handleProductAddClick(product);
  }

  async populateCategories() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/categories', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ categories: data, categoriesLoading: false });
  }

  async populateProducts(id) {
    const token = await authService.getAccessToken();
    let reqUrl = '';

    if (id === 0) {
      reqUrl = 'api/products';
    } else {
      reqUrl = 'api/products/category/' + id;
    }

    const response = await fetch(reqUrl, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    const data = await response.json();
    this.setState({ products: data, productsLoading: false });
  }

  render() {
    return (
      <div>
        <Categories categoriesLoading={this.state.categoriesLoading} categoriesList={this.state.categories} handleCategoriesClick={this.handleCategoriesClick} />
        <CSSTransition classNames="products"
          in={!this.state.productsLoading}
          timeout={300}
        >
          <ProductCars
            productsLoading={this.state.productsLoading} products={this.state.products} handleProductAddClick={this.handleProductAddClick} />
        </CSSTransition>
      </div>
    );
  }
}
