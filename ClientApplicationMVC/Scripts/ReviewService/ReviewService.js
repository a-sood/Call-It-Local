
/* This is for search bar validation */
function validateSearch() {
    var response = true;

    if (searchForm.searchTextBox.value === "") {
        document.getElementById("searchError").innerHTML = "Please enter some text to search";
        var searchDiv = document.getElementById("searchResult");
        if (searchDiv != null) {
            document.getElementById("searchResult").innerHTML = "";
        }
        response = false;
    }
    else if (searchForm.searchTextBox.value.match(/[^\w ]/)) {
        document.getElementById("searchError").innerHTML = "You can only use alphanumeric characters for a name search";
        var searchDiv = document.getElementById("searchResult");
        if (searchDiv != null) {
            document.getElementById("searchResult").innerHTML = "";
        }
        response = false;
    }
    else {
        document.getElementById("searchError").innerHTML = "";
    }


    return response;
}

function validateReview() {
    var response = true;

    if (reviewForm.reviewTextBox.value === "") {
        document.getElementById("reviewError").innerHTML = "Please enter some text for review";
        response = false;
    }
    else if (reviewForm.reviewTextBox.value.match(/[^\w .,_@!:;-]/)) {
        document.getElementById("reviewError").innerHTML = "You can not use special characters in the review text";
        response = false;
    }
    else {
        document.getElementById("reviewError").innerHTML = "";
    }


    return response;
}
