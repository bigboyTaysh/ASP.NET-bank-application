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
    margin: theme.spacing(0.5),
  },
}));

export default function Categories({ categoriesList }) {
  const classes = useStyles();

  const handleClick = () => {
    console.log("click")
  };

  return (
    <Paper 
      component="ul"
      className={classes.root}
      >
      {categoriesList.map((data) => {
        return (
          <li key={data.id}>
            <Chip
              label={data.name}
              //onDelete={data.label === 'React' ? undefined : handleDelete(data)}
              onClick={handleClick}
              className={classes.chip}
            />
          </li>
        );
      })}
    </Paper>
  );
}