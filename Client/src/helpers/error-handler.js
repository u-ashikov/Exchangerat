function extractErrorsFromResponse(errorResponse) {
    var errors = [];

    if (errorResponse && errorResponse.status == 400) {
        if (errorResponse.data.errors) {
          Object.values(errorResponse.data.errors).map(function(e) {
            errors = errors.concat(e);
          });
        } else if (errorResponse.data) {
          errors = errorResponse.data;
        }
    } else {
        errors.push("Sorry, it seems that an error occured.");
    }

    return errors;
}

export default {
  extractErrorsFromResponse
}