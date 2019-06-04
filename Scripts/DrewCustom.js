//Navbar Active Highlight Functionality
//The nav-list class is attached to the <ul>
$('.nav-list').on('click', 'li', function () {
    //Remove the Active class from the <li> that is Active
    $('.nav-list li.active').removeClass('active');

    //Add the Active class to the <li> that was clicked
    $(this).addClass('active');
});
