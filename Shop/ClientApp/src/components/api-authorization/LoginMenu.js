import React, { Component, Fragment } from 'react';
import { NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import authService from './AuthorizeService';
import { ApplicationPaths } from './ApiAuthorizationConstants';
import { MenuItem } from '@material-ui/core';

export class LoginMenu extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isAuthenticated: false,
            userName: null
        };
    }

    componentDidMount() {
        this._subscription = authService.subscribe(() => this.populateState());
        this.populateState();
    }

    componentWillUnmount() {
        authService.unsubscribe(this._subscription);
    }

    async populateState() {
        const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()])
        this.setState({
            isAuthenticated,
            userName: user && user.name
        });
    }

    render() {
        const { isAuthenticated, userName } = this.state;
        if (!isAuthenticated) {
            const registerPath = `${ApplicationPaths.Register}`;
            const loginPath = `${ApplicationPaths.Login}`;
            return this.anonymousView(registerPath, loginPath);
        } else {
            const profilePath = `${ApplicationPaths.Profile}`;
            const logoutPath = { pathname: `${ApplicationPaths.LogOut}`, state: { local: true } };
            return this.authenticatedView(userName, profilePath, logoutPath);
        }
    }

    authenticatedView(userName, profilePath, logoutPath) {
        return (<Fragment>
            <MenuItem>
                <NavLink tag={Link} className="text-dark" to={profilePath} onClick={this.props.onClick}>Hello {userName}</NavLink>
            </MenuItem>
            <MenuItem>
                <NavLink tag={Link} className="text-dark" to={logoutPath} onClick={this.props.onClick}>Logout</NavLink>
            </MenuItem>
        </Fragment>);

    }

    anonymousView(registerPath, loginPath) {
        return (<Fragment>
            <MenuItem>
                <NavLink tag={Link} className="text-dark" to={loginPath} onClick={this.props.onClick}>Zaloguj się</NavLink>
            </MenuItem>
            <MenuItem>
                <NavLink tag={Link} className="text-dark" to={registerPath} onClick={this.props.onClick}>Zarejestruj się</NavLink>
            </MenuItem>
        </Fragment>);
    }
}
