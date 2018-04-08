function setActiveMenu(activeMenuLink) {
    if (activeMenuLink)
        $(`#${activeMenuLink}`).parent().addClass('active');
}