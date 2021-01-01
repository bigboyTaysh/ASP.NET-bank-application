import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Chip from '@material-ui/core/Chip';
import Paper from '@material-ui/core/Paper';
import TagFacesIcon from '@material-ui/icons/TagFaces';
import { createMuiTheme } from '@material-ui/core/styles';
import red from '@material-ui/core/colors/red';
import authService from './api-authorization/AuthorizeService';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    justifyContent: 'center',
    flexWrap: 'wrap',
    listStyle: 'none',
    padding: theme.spacing(0.5),
    margin: 0,
    backgroundColor: "#3d3d3d",
  },
  chip: {
    fontSize: 30,
    margin: theme.spacing(0.5),
  },
}));

export default function Categories(props) {
  const classes = useStyles();

  const handleClick = (id) => {
    props.handleCategoriesClick(id);
  };

  return (
    <Paper
      component="ul"
      className={classes.root}
    >
      <Chip
        label={"Wszystkie"}
        //onDelete={data.label === 'React' ? undefined : handleDelete(data)}
        onClick={() => handleClick(0)}
        className={classes.chip}
      />
      {props.categoriesList.map((data) => {
        return (
          <li key={data.id}>
            <Chip
              label={data.name}
              //onDelete={data.label === 'React' ? undefined : handleDelete(data)}
              onClick={() => handleClick(data.id)}
              className={classes.chip}
            />
          </li>
        );
      })}
    </Paper>
  );
}