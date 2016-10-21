  function DisplayProgressMessage(ctl, msg) {
      $(ctl).prop("disabled", true);
      $(ctl).text(msg);
      return true;
  }

