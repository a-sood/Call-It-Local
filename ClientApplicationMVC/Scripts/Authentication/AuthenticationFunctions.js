// To be completed by students in milestone 2
/*
 * Validates the username and password entered into the login page.
 * If acceptable, the method returns true, else it returns false.
 */
function validateLoginForm() {
    let showUsernameError, showPasswordError = false;

    /* EMPTY USERNAME */
    if (asIsLoginForm.usernameText.value === '') {
        showUsernameError = true;
        document.getElementById('usernameError').innerHTML = '*required';
    }

    /* EMPTY PASSWORD */
    if (asIsLoginForm.passwordText.value === '') {
        showPasswordError = true;
        document.getElementById('passwordError').innerHTML = '*required';
    }

    /* INVALID USERNAME */
    if (asIsLoginForm.usernameText.value.match(/[\W_]/)) {
        document.getElementById("usernameError").innerHTML = "You can only use alphanumeric characters for a username";
        showUsernameError = true;
    }

    /* INVALID PASSWORD */
    if (asIsLoginForm.passwordText.value.match(/[^\w$!@_]/)) {
        document.getElementById("passwordError").innerHTML = "You can only use alphanumeric, @, $, !, _ characters for a password";
        showPasswordError = true;
    }

    /* ACCEPTED LOGIN INFO */
    if (!showUsernameError)
        document.getElementById('usernameError').innerHTML = '';
    if (!showPasswordError)
        document.getElementById('passwordError').innerHTML = '';
    if (!(showUsernameError || showPasswordError))
        return true;

    return false;
}

function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function runthis() {
    var response = true;

    var email = createAccountForm.email.value;
    var atpos = email.indexOf("@");
    var dotpos = email.lastIndexOf(".");
    var phoneNum = /^([0-9]{3})[-]?([0-9]{3})[-]?([0-9]{4})$/;
    

    if (createAccountForm.email.value === "") {
        document.getElementById("emailError").innerHTML = "Please enter an email";
        response = false;
    }
    else if (!validateEmail(email) || atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= email.length) {
        document.getElementById("emailError").innerHTML = "Please enter a valid email";
        response = false;
    }
    else {
        document.getElementById("emailError").innerHTML = "";
    }

    if (createAccountForm.password.value === "") {
        document.getElementById("passwordError").innerHTML = "Please enter a password";
        response = false;
    }
    else if (createAccountForm.password.value.length < 8)
    {
        document.getElementById("passwordError").innerHTML = "Password must be atleast 8 characters";
        response = false;
    }
    else if (createAccountForm.password.value.match(/[^\w$!@_]/)) {
        document.getElementById("passwordError").innerHTML = "You can only use alphanumeric, @, $, !, _ characters for a password";
        response = false;
    }
    else {
        document.getElementById("passwordError").innerHTML = "";
    }

    if (createAccountForm.phone.value === "") {
        document.getElementById("phoneError").innerHTML = "Please enter a phone number";
        response = false;
    }
    else if (!createAccountForm.phone.value.match(phoneNum)) {
        document.getElementById("phoneError").innerHTML = "Please enter a valid phone number, format xxx-xxx-xxxx";
        response = false;
    }
    else { 
        document.getElementById("phoneError").innerHTML = "";
    }

    if (createAccountForm.username.value === "") {
        document.getElementById("usernameError").innerHTML = "Please enter an username";
        response = false;
    }
    else if (createAccountForm.username.value.match(/[\W_]/))
    {
        document.getElementById("usernameError").innerHTML = "You can only use alphanumeric characters for a username";
        response = false;
    }
    else {
        document.getElementById("usernameError").innerHTML = "";
    }

    if (createAccountForm.name.value === "") {
        document.getElementById("nameError").innerHTML = "Please enter a name";
        response = false;
    }
    else if (createAccountForm.name.value.match(/[^\w ]/)) {
        document.getElementById("nameError").innerHTML = "You can only use alphanumeric characters for a name";
        response = false;
    }
    else {
        document.getElementById("nameError").innerHTML = "";
    }

    if (createAccountForm.address.value === "") {
        document.getElementById("addressError").innerHTML = "Please enter an address";
        response = false;
    }
    else if (createAccountForm.address.value.match(/[^\w .,-]/)) {
        document.getElementById("addressError").innerHTML = "You can not use invalid characters for address";
        response = false;
    }
    else {
        document.getElementById("addressError").innerHTML = "";
    }

    return response;
}
