
  //Chart Functions: 
  function drawBarChart(elementId, title, subtitle, dataTable) {
      var options = {
          chart: {
              title: title,
              subtitle: subtitle
          }
      };
      var chart = new google.visualization.Bar(document.getElementById(elementId));
      chart.draw(dataTable, options);
  }

  function drawColumnChart(elementId, title, hAxis,vAxis, dataTable) {
      var options = {
          title: title,
          hAxis: { title: hAxis },
          vAxis: { title: vAxis }
      };
      var chart = new google.visualization.ColumnChart(document.getElementById(elementId));
      chart.draw(dataTable, options);
  }
  