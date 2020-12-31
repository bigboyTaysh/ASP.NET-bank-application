import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService';
import Categories from './Categories';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = { 
      categories: [],
      products: [],
      categoriesLoading: true,
      productsLoading: true
    };
  }

  componentDidMount () {
    this.populateCategories();
    this.populateProducts(0);
  }

  handleClick = (value) => {
    this.populateProducts(value);
  }

  async populateCategories () {
    const token = await authService.getAccessToken();
    const response = await fetch('api/categories', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ categories: data, categoriesLoading: false });
  }

  async populateProducts (id) {
    const token = await authService.getAccessToken();
    let reqUrl = '';
    if(id === 0){
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

  render () {
    return (
      <Categories categoriesList={this.state.categories} handleClick={this.handleClick}/>
    );
  }
}
