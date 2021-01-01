import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService';
import Categories from './Categories';
import Cards from './Card';
import Grid from '@material-ui/core/Grid';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = {
      categories: [],
      products: [],
      itemsCount: 0,
      basket: [],
      categoriesLoading: true,
      productsLoading: true
    };
  }

  componentDidMount() {
    this.populateCategories();
    this.populateProducts(0);
  }

  handleCategoriesClick = (value) => {
    this.populateProducts(value);
  }

  handleProductAddClick = () => {
    this.props.handleProductAddClick();
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
        <Categories categoriesList={this.state.categories} handleCategoriesClick={this.handleCategoriesClick} />
        <Grid container justify="center" spacing={3}>
          {this.state.products.map((item) => (
            <Grid key={item.id} item>
              <Cards product={item} handleProductAddClick={this.handleProductAddClick}/>
            </Grid>
          ))}
        </Grid>
      </div>
    );
  }
}
