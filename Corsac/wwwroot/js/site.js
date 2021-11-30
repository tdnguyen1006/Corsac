$(document).ready(function() {
  var mainTable;

  mainTable = $(".table").DataTable({
    lengthChange: false,
    searching: true,
    dom: "lrtip",
    stateLoadParams: function(settings, data) {
      $("#mainSearch").val(data.search.search);
    },
    scrollX: false
  });

  $("#mainSearch").keyup(function() {
    mainTable.search($(this).val()).draw();
  });
});
