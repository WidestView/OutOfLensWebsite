//NAV
$(".navbar-nav .nav-item").on("click", function () {
	$(".nav-item").removeClass("active");
	$(this).addClass("active");
});

$('#loginDropdownButton').click(() => {
	$('#loginDropdown').fadeToggle();
	if ($('#loginDropdownButton').hasClass('btn-primary')) {
		$('#loginDropdownButton').html('Fechar');
		$('#loginDropdownButton').removeClass('btn-primary');
		$('#loginDropdownButton').addClass('btn-danger');
	} else {
		$('#loginDropdownButton').html('Entrar');
		$('#loginDropdownButton').removeClass('btn-danger');
		$('#loginDropdownButton').addClass('btn-primary');
	}
});
//PAGE

//FOOTER

//MODALS

//EXTRA