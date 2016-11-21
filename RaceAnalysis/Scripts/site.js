
 

  function drawColumnChart(elementId, title, hAxis,vAxis, dataTable) {
      var options = {
          title: title,
          hAxis: { title: hAxis },
          vAxis: { title: vAxis }
      };
      var chart = new google.visualization.ColumnChart(document.getElementById(elementId));
      chart.draw(dataTable, options);
  }
  