const frmContact = document.getElementById("frmContact");
const Contact_Name = document.getElementById("Contact_Name");
const Email_Address = document.getElementById("Email_Address");
const Phone_No = document.getElementById("Phone_No");
const r1 = document.getElementById("r1");
const r2 = document.getElementById("r2");
const acch = document.forms["frmContact"]["Account_holder"].value;
//var getSelectedRadioValue = document.querySelector('input[name="Account_holder"]:checked');
const Preferred_Branch = document.getElementById("Preferred_Branch");
const Query_Type = document.getElementById("Query_Type");
const Message = document.getElementById("Message");
const privacy_check = document.getElementById("privacy_check");

let isFormValid = false;
frmContact.addEventListener('submit', (e) => {
    e.preventDefault(); //to prevent the form from submitting by default.
    
    validateInputs();
    //if (isFormValid) {
    //    frmContact.remove();
    //}
    
});

const setError = (element, message) => {
    const inputControl = element.parentElement;
    const errorDisplay = inputControl.querySelector('.form-error');
    errorDisplay.innerText = message;
    inputControl.classList.add('error');
    inputControl.classList.remove('success');
    //isFormValid = true;
};
const setSuccess = (element) => {
    const inputControl = element.parentElement;
    const errorDisplay = inputControl.querySelector('.form-error');
    errorDisplay.innerText = '';
    inputControl.classList.add('success');
    inputControl.classList.remove('error');
    //isFormValid = false;
};

const isValidEmail = (email) => {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
};
const isValidPhone = (phone) => {
    //const re = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
    //           /^[\+]?[(]?[0-9]{1,2}[)]?[0-9]{3}[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/  //Example for Valid Phone No:+(91)123-456-7890
    //           /^[\+]?[(]?[0-9]{1,2}[)]?[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$/

    //New:         /^[\+]?[(]*[0-9][)]*[-\s]*[(]*[0-9][(]*[-\s]*[(]*[0-9][(]*[-\s]*[(]*[0-9][(]*$/

    const re = /^[0-9]{10}$/;
    return re.test(String(phone).toLowerCase());
};

const resetElement = (element) => {
    element.classList.remove('error');
};


const validateInputs = () => {
    const Contact_Name_Value = Contact_Name.value.trim();
    const Email_Address_Value = Email_Address.value.trim();
    const Phone_No_Value = Phone_No.value.trim();
    var msglen = Message.value.length;
    

    //Validate Contact Name
    if (Contact_Name_Value === '' || Contact_Name_Value === null) {
        setError(Contact_Name, 'Name is required');
    }
    else if (!isNaN(Contact_Name_Value)) {
        setError(Contact_Name, 'Name must contain characters');
    }
    else {
        setSuccess(Contact_Name);
    }

    //Validate Email ID & Phone No fields

    //if (!isValidEmail(Email_Address_Value)) {
    //    setError(Email_Address, 'Please Enter a valid Email address');
    //}
    //else if (!isValidPhone(Phone_No_Value)) {
    //    setError(Phone_No, 'Please enter valid Phone No');
    //}
    //else if (Email_Address_Value === '' && Phone_No_Value !=='') {
    //    setSuccess(Email_Address);
    //    setSuccess(Phone_No);
    //    resetElement(Email_Address);
    //    resetElement(Phone_No);
    //}
    //else if (Email_Address_Value !== '' && Phone_No_Value === '') {
    //    setSuccess(Email_Address);
    //    setSuccess(Phone_No);
    //    resetElement(Email_Address);
    //    resetElement(Phone_No);
    //}
    // else {
    //    setError(Email_Address, 'Email Address is required');
    //    setError(Phone_No, 'Phone No is required');
    //}

    //Validate Account Holder Radiobutton 
    if (!r1.checked && !r2.checked) {
        //setError(r1, 'Please Select Yes or No');
        document.getElementById("radio-div").innerHTML = "Please choose an option";
    }
    else {
        setSuccess(r1);
    }

    //Validate Preferred_Branch
    if (Preferred_Branch.value === '') {
        setError(Preferred_Branch, 'Please select Preferred branch');
        //Preferred_Branch.focus();
    }
    else {
        setSuccess(Preferred_Branch);
    }

    //Validate Query Type
    if (Query_Type.value === '') {
        setError(Query_Type, 'Please select Type of Query');
    }
    else {
        setSuccess(Query_Type);
    }

    //Validate Message for Minimum 30 chars
    if (msglen > 0 && msglen < 30) {
        setError(Message, 'Message should be minimum 30 characters');
        //Message.focus();
    }
    else if (Message.value === '') {
        setError(Message, 'Please enter Your Message');
        //Message.focus();
    }
    else {
        setSuccess(Message);
    }

    //Validate Privacy Checkbox
    if (!privacy_check.checked) {
        setError(privacy_check, "Please select the Privacy checkbox.");
    }
    else {
        setSuccess(privacy_check);
    }
};