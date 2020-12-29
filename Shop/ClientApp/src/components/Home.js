import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService';
import Categories from './Categories';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = { categories: [], loading: true };
  }

  componentDidMount () {
    this.populateCategories();
  }

  handleClick = (value) => {

  }

  async populateCategories () {
    const token = await authService.getAccessToken();
    const response = await fetch('api/categories', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ categories: data, loading: false });
  }

  render () {
    return (
      <Categories categoriesList={this.state.categories} handleClick={this.handleClick}/>
    );
  }
}
