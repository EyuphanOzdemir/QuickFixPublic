import React, {useContext, useRef} from "react";
import EasyGrid from "../components/commonComponents/easyGrid";
import config from "../config";



const ShowMyFixes = ({author}) => {
  console.log("start in ShowMyFixes");
 
  return (
    <EasyGrid
      columns={[
        {
          field: "id",
          width: 150,
          headerName: "Id",
        },
        {
          field: "category",
          headerName: "Category",
          width: 200,
        },
        {
          field: "title",
          width: 300,
          headerName: "Title",
        },
        { 
          field: 'displayLink', 
          headerName: 'Display', 
          width: 200, 
          renderCell: (params) => {
            console.log('params:', params); // Debugging statement
            console.log('params.value:', params.id); // Debugging statement
      
            return (
              <a href={`${config.webBaseUrl}/Fix/DisplayFix?Id=${params.id}`} target="_blank" rel="noopener noreferrer">
                Display
              </a>
            );
          }},
          { 
            field: 'editLink', 
            headerName: 'Edit', 
            width: 200, 
            renderCell: (params) => {
              console.log('params:', params); // Debugging statement
              console.log('params.value:', params.id); // Debugging statement
        
              return (
                <a href={`${config.webBaseUrl}/Fix/EditFix?Id=${params.id}`} target="_blank" rel="noopener noreferrer">
                  Edit
                </a>
              );
          } }
      ]}
      apiFunction= {async (page, pageSize, search, selectValues) => {
        try {
          page += 1;
          var fetchUrl = `${config.fixApiBaseUrl}/Fix/Search?Author=${author}&PageId=${page}`
          console.log("fetch url:", fetchUrl);
          const response = await fetch(fetchUrl);
          if (!response.ok) {
            throw new Error('Network response was not ok');
          }
          const result = await response.json();
          return result;
        } catch (error) {
          console.error("There was a problem with the fetch operation:", error);
        }
      }}
      mainWidth={1000}
      enableSearch={false}
      searchHelper="Search for question text, VVV (correct) or XXX (wrong), category name, difficulty level, answer, right answer"
      enableCellExpand
      enableRefresh={false}
      selectLists={
        [
          
        ]
      }
      resultField = "result"
      listField = "fixes"
      successField = "isSuccess"
      messageField="message"
      countField = "count"
    />
  );
};

export default ShowMyFixes;