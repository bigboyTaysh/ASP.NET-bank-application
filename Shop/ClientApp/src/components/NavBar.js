import React from 'react';
import { fade, makeStyles } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import IconButton from '@material-ui/core/IconButton';
import Typography from '@material-ui/core/Typography';
import InputBase from '@material-ui/core/InputBase';
import Badge from '@material-ui/core/Badge';
import MenuItem from '@material-ui/core/MenuItem';
import Menu from '@material-ui/core/Menu';
import MenuIcon from '@material-ui/icons/Menu';
import SearchIcon from '@material-ui/icons/Search';
import AccountCircle from '@material-ui/icons/AccountCircle';
import MailIcon from '@material-ui/icons/Mail';
import BasketIcon from '@material-ui/icons/ShoppingBasket';
import MoreIcon from '@material-ui/icons/MoreVert';
import { LoginMenu } from './api-authorization/LoginMenu';
import { NavLink } from 'react-router-dom';
import { Link } from '@material-ui/core';
import Popper from '@material-ui/core/Popper';
import Paper from '@material-ui/core/Paper';
import Fade from '@material-ui/core/Fade';

const useStyles = makeStyles((theme) => ({
  grow: {
    flexGrow: 1,
  },
  title: {
    display: 'none',
    [theme.breakpoints.up('sm')]: {
      display: 'block',
    },
  },
  menuButton: {
    marginRight: theme.spacing(2),
  },
  sectionDesktop: {
    display: 'none',
    [theme.breakpoints.up('md')]: {
      display: 'flex',
    },
  },
  sectionMobile: {
    display: 'flex',
    [theme.breakpoints.up('md')]: {
      display: 'none',
    },
  },
  link: {
    marginLeft: theme.spacing(10),
    color: "#f8f9fa",
    '&:hover': {
      color: "#f8f9fa",
      textDecoration: "none",
    },
  },
  rightMenu: {
    marginRight: theme.spacing(10)
  }, 
  typography: {
    padding: theme.spacing(3),
  },
}));

export default function NavBar(props) {
  const classes = useStyles();
  const [anchorLoginEl, setAnchorLoginEl] = React.useState(null);
  const [anchorBasketEl, setAnchorBasketEl] = React.useState(null);
  const [mobileMoreAnchorEl, setMobileMoreAnchorEl] = React.useState(null);

  const isMenuOpen = Boolean(anchorLoginEl);
  const isMobileMenuOpen = Boolean(mobileMoreAnchorEl);
  const menuId = 'primary-search-account-menu';

  const isBasketOpen = Boolean(anchorBasketEl);
  const idBasket = isBasketOpen ? 'simple-popper' : undefined;

  const data = props.data;

  const handleProfileMenuOpen = (event) => {
    setAnchorLoginEl(event.currentTarget);
  };

  const handleMobileMenuClose = () => {
    setMobileMoreAnchorEl(null);
  };

  const handleMenuClose = () => {
    setAnchorLoginEl(null);
    handleMobileMenuClose();
  };

  const handleMobileMenuOpen = (event) => {
    setMobileMoreAnchorEl(event.currentTarget);
  };

  const handleClickBasket = (event) => {
    setAnchorBasketEl(anchorBasketEl ? null : event.currentTarget);
  };

  const renderMenu = (
    <Menu
      anchorEl={anchorLoginEl}
      anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
      id={menuId}
      keepMounted
      transformOrigin={{ vertical: 'top', horizontal: 'right' }}
      open={isMenuOpen}
      onClose={handleMenuClose}
    >
      <LoginMenu onClick={handleMenuClose} />
    </Menu>
  );

  const mobileMenuId = 'primary-search-account-menu-mobile';
  const renderMobileMenu = (
    <Menu
      anchorEl={mobileMoreAnchorEl}
      anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
      id={mobileMenuId}
      keepMounted
      transformOrigin={{ vertical: 'top', horizontal: 'right' }}
      open={isMobileMenuOpen}
      onClose={handleMobileMenuClose}
    >
      <LoginMenu onClick={handleMenuClose} />
    </Menu>
  );

  const renderBasket = (
    <Popper
      open={isBasketOpen}
      id={idBasket}
      anchorEl={anchorBasketEl}
      placement="bottom"
      disablePortal={false}
      transition
    >
      {({ TransitionProps }) => (
        <Fade {...TransitionProps} timeout={350}>
          <Paper>
            <Typography
            className={classes.typography}
            variant="h5"
            >
              {(data.itemsCount > 0 ? 'Całkowity koszt: ' + data.basket.reduce((a,b) => a + b.price, 0) + ' zł' : 'Koszyk jest pusty')}
            </Typography>
          </Paper>
        </Fade>
      )}
    </Popper>
  );

  return (
    <div className={classes.grow}>
      <AppBar
        position="static"
        color="secondary"
      >
        <Toolbar>
          <Typography className={classes.title} variant="h3" noWrap>
            <NavLink to="/" className={classes.link}>
              Giga sklep
            </NavLink>
          </Typography>
          <div className={classes.grow} />
          <IconButton aria-label="show new items in basket" color="inherit" onMouseEnter={handleClickBasket} onMouseLeave={handleClickBasket}>
            <Badge badgeContent={data.itemsCount} color="secondary">
              <BasketIcon fontSize="large" />
            </Badge>
          </IconButton>
          <div className={classes.sectionDesktop}>
            <IconButton
              edge="end"
              aria-label="account of current user"
              aria-controls={menuId}
              aria-haspopup="true"
              onClick={handleProfileMenuOpen}
              color="inherit"
              className={classes.rightMenu}
            >
              <AccountCircle fontSize="large" />
            </IconButton>
          </div>
          <div className={classes.sectionMobile}>
            <IconButton
              aria-label="show more"
              aria-controls={mobileMenuId}
              aria-haspopup="true"
              onClick={handleMobileMenuOpen}
              color="inherit"
            >
              <MoreIcon />
            </IconButton>
          </div>
        </Toolbar>
      </AppBar>
      {renderMobileMenu}
      {renderMenu}
      {renderBasket}
    </div>
  );
}
