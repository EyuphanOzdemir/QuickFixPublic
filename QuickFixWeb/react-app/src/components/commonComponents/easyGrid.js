import React, { useRef } from "react";
import { DataGrid } from "@material-ui/data-grid";
import Select from "@material-ui/core/Select";
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import MenuItem from "@material-ui/core/MenuItem";
import TextField from "@material-ui/core/TextField";
import { makeStyles } from "@material-ui/core/styles";
import FormHelperText from "@material-ui/core/FormHelperText";
import Button from "@material-ui/core/Button";
import { stringMethods } from "../../modules/stringMethods";
import _ from 'lodash';
import SelectList from "./selectList";

const useStyles = makeStyles((theme) => ({
  formControl: {
    margin: theme.spacing(1),
    minWidth: 150,
  },
}));

const EasyGrid = ({
  apiFunction,
  mainWidth,
  enableSearch,
  selectLists,
  enableRefresh,
  columns,
  searchHelper,
  enableCellExpand,
  resultField,
  listField,
  countField,
  messageField,
  successField
}) => {
  console.log("start in Easy Grid");
  console.log("select lists", selectLists);
  const classes = useStyles();
  const searchRef = useRef(null);
  const pageSizeRef = useRef(null);

  const [page, setPage] = React.useState(0);
  const [rows, setRows] = React.useState([]);
  const [loading, setLoading] = React.useState(false);
  const [pageSize, setPageSize] = React.useState(10);
  const [totalRowCount, setTotalRowCount] = React.useState(0);
  const [error, setError] = React.useState(null);
  const [search, setSearch] = React.useState("");
  
  const showSelectLists = selectLists && selectLists.length > 0;
  let selectListCount=0;
  if (showSelectLists) selectListCount=selectLists.length;
  let initialSelectValues=[];
  if (showSelectLists) initialSelectValues=_.fill(Array(selectListCount), "");
  const [selectValues, setSelectValues]=React.useState(initialSelectValues);

  const updateSelectValue=(index, value)=>{
    console.log("in update select value", index, value);
    setSelectValues((previousValues) => {
      let newValues = [ ...previousValues ];
      newValues[index] = value;
      return newValues;
    });
  }

  const loadServerRows = React.useCallback(async () => {
    console.log("Loading data...");
    const result = await apiFunction(
        page,
        pageSize,
        searchRef?.current?.value,
        selectValues
      );
    console.log("Loaded");
    if (result[successField]) {
      setError(null);
      return result[resultField];
    } else {
      setError(result[messageField]);
      return null;
    }
  }, [apiFunction, page, pageSize, selectValues]);

  const handlePageChange = (page) => {
    setPage(page);
  };

  const loadData = React.useCallback(async () => {
    setLoading(true);
    let result = await loadServerRows();
    if (result) {
      let rowCount = result[countField];
      let rows = result[listField];
      setTotalRowCount(rowCount);
      setRows(rows);
    }
    setLoading(false);
  }, [loadServerRows, countField, listField]);

  const handleRefresh = async (e) => {
    e.preventDefault();
    loadData();
    if (page !== 0) setPage(0);
  };

  React.useEffect(() => {
    (async () => await loadData())();
  }, [loadData]);


  console.log("Form control class", classes.formControl)

  return (
    <div className="container">
      <div id="refreshBar">
        {showSelectLists && (
          <>
          {selectLists.map((selectList, index)=>
            (<SelectList 
            formControlClassName={classes.formControl}
            id={index}
            selectList={selectList}
            onChangeMethod={updateSelectValue}
            />)
          )}
          </>
        )}

        {enableSearch && (
            <FormControl className={classes.formControl}>
              <TextField
                variant="outlined"
                value={search}
                label="Search"
                placeholder="0 or at least 3 characters..."
                onChange={(e) => setSearch(e.target.value)}
                inputRef={searchRef}
                title={searchHelper}
              />
            </FormControl>
        )}

        {(enableRefresh || enableSearch) && (
            <FormControl className={classes.formControl}>
              <Button
                variant="contained"
                color="primary"
                type="submit"
                disabled={loading}
                onClick={handleRefresh}
                className="btn-block"
              >
                {loading && (
                  <span className="spinner-border spinner-border-sm"></span>
                )}
                <span>Refresh</span>
              </Button>
            </FormControl>
        )}
      </div>
      
      <div>
        <FormControl className={classes.formControl}>
          <FormHelperText>{error && <span>{error}</span>}</FormHelperText>
        </FormControl>
      </div>
      
      <div id="gridContainer" style={{ width: mainWidth }}>
        <DataGrid
          autoHeight={true}
          columns={columns.map((column) => ({
            ...column,
            disableClickEventBubbling: true,
            renderHeader: () => stringMethods.makeStrong(column.headerName),
            //renderCell: renderCell(),
            cellClassName: "dataGridCell",
          }))}
          rows={rows}
          pagination
          pageSize={pageSize}
          rowCount={totalRowCount}
          paginationMode="server"
          onPageChange={handlePageChange}
          loading={loading}
          error={error}
          rowsPerPageOptions={[]}
          page={page}
        />
      </div>
    </div>
  );
};

export default EasyGrid;
