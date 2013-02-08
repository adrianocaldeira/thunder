(function ($) {
    var manager;
    $.manager = manager = {
        root: '',
        utility: {
            applyAjaxForm: function(form, options) {
                var $form = $(form);
                var settings = $.extend({ }, {
                    onSuccess: function() {
                    },
                    onBefore: function() {
                    },
                    submitButton: $('#submit'),
                    loading: $('#loading'),
                    modal: false
                }, options);

                if (settings.modal && settings.submitButton != null) {
                    $.thunder.submitButton(settings.submitButton, $form);
                }

                $form.ajaxForm({
                    loading: settings.loading,
                    onBeforeSubmit: function() {
                        settings.onBefore.call($form);
                    },
                    onSuccess: function(f, r) {
                        settings.onSuccess.call($form, r);
                    }
                });
            }
        },
        login: {
            index: function() {
                $('form').ajaxForm({
                    message: $('#message'),
                    onSuccess: function(f, r) {
                        window.location.href = r.Data.Path;
                    }
                });
            }
        },
        stay: function(url) {

            var connected = function() {
                $.ajax({
                    type: 'post',
                    url: url,
                    success: function() {
                        window.setTimeout(connected, 30000);
                    }
                });
            };

            window.setTimeout(connected, 30000);
        },
        userProfiles: {
            index: function() {
                var $grid = $('#grid-profiles');

                $grid.grid();

                $.thunder.applyDelete($('a.delete', $grid), {
                    success: function() {
                        $.manager.userProfiles.list();
                    }
                });
            },
            list: function() {
                $('form', '#grid-profiles').submit();
            },
            form: function() {
                $.manager.utility.applyAjaxForm('#user-profile', {
                    onBefore: function() {
                        var $form = $(this);
                        $form.find('.functionality').remove();
                        $.each($('.permission:input:checkbox:checked'), function(i) {
                            $form.append('<input type="hidden" class="functionality" name="Functionalities[' + i + '].Id" value="' + $(this).val() + '" />');
                        });
                    },
                    onSuccess: function(r) {
                        window.location.href = r.Data;
                    }
                });

                $('a.permission').click(function(e) {
                    var $this = $(this);
                    var $dialog = $('#permission-modal-' + $this.data('module'));

                    $dialog.dialog({
                        modal: true,
                        title: 'Permissões'
                    });

                    e.preventDefault();
                });

                $('.tree-view input:checkbox:not(.permission)').click(function() {
                    var $checkbox = $(this);
                    var $li = $checkbox.parents('li:first');
                    var $childs = $('input:checkbox', $li);
                    var $parents = $('input:checkbox:first', $li.parents('li'));

                    if ($checkbox.is(':checked')) {
                        $parents.attr('checked', 'checked');
                        $childs.attr('checked', 'checked');
                        $.each($childs, function() {
                            var $this = $(this);
                            var $parentChild = $this.parents('li:first');
                            var $dialog = $('#permission-modal-' + $parentChild.data('module'));
                            $('input:checkbox', $dialog).attr('checked', 'checked');
                        });
                    } else {
                        $childs.removeAttr('checked');
                        $('input:checkbox', $('#permission-modal-' + $li.data('module'))).removeAttr('checked');
                        $.each($childs, function() {
                            var $this = $(this);
                            var $parentChild = $this.parents('li:first');
                            var $dialog = $('#permission-modal-' + $parentChild.data('module'));
                            $('input:checkbox', $dialog).removeAttr('checked');
                        });

                    }
                });

                $('.tree-view input.permission:checkbox').click(function() {
                    var $this = $(this);
                    var $dialog = $this.parents('.ui-dialog-content');
                    var $li = $('li[data-module="' + $dialog.data('module') + '"]');
                    var $module = $('input:checkbox:first', $li);

                    if ($this.is(':checked')) {
                        $('input:checkbox:first', $module.parents('li')).attr('checked', 'checked');
                    } else {
                        if ($('input:checkbox:checked', $dialog).size() == 0) {
                            $module.removeAttr('checked');
                        }
                    }
                });
            }
        },
        users: {
            index: function() {
                var $grid = $('#grid-users');

                $grid.grid();

                $('#add-user, #grid-users a.edit').live('click', function(e) {
                    $.modal({
                        iframe: true,
                        url: $(this).attr('href'),
                        width: 900,
                        height: 500
                    });
                    e.preventDefault();
                });

                $.thunder.applyDelete($('a.delete', $grid), {
                    success: function() {
                        $.manager.users.list();
                    }
                });
            },
            list: function() {
                $('form', '#grid-users').submit();
            },
            form: function() {
                $.manager.utility.applyAjaxForm('#user', {
                    modal: true,
                    onSuccess: function() {
                        window.parent.$.manager.users.list();
                        window.parent.$.closeModal();
                    }
                });
            }
        },
        news: {
            index: function() {
                var $grid = $('#news');

                $grid.grid();

                $.thunder.applyDelete($('a.delete', $grid), {
                    success: function() {
                        $.manager.news.list();
                    }
                });
            },
            list: function() {
                $('form', '#news').submit();
            },
            form: function() {
                $.manager.utility.applyAjaxForm('#form-news', {
                    onSuccess: function(r) {
                        window.location.href = r.Data;
                    }
                });
            }
        }
    };
    
    $(function () {
        manager.root = $('body').data('root');

        $.thunder.settings.images.loadingModal = manager.root + 'content/jquerythunder/images/loading_modal.gif';
        $.thunder.settings.images.loadingGrid = manager.root + 'content/jquerythunder/images/loading_grid.gif';
        $.thunder.settings.images.loadingForm = manager.root + 'content/jquerythunder/images/loading_form.gif';
        $.thunder.settings.confirm.label.yes = 'Sim';
        $.thunder.settings.confirm.label.no = 'Não';

        $.datepicker.setDefaults($.datepicker.regional['pt-BR']);

        $('.date').datepicker({
            showOn: 'button',
            buttonImage: manager.root + 'content/manager/images/icons/calendar.png',
            buttonImageOnly: true
        });

        $('.numeric').live('keydown', function (event) {
            if (!(event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13)) {
                if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();
                }
            }
        });

        $('.date').mask('99/99/9999');
        $('.date-time').mask('99/99/9999 99:99?:99');
        $('.time').mask('99:99');
        $('.cpf').mask('999.999.999-99');
        $('.cnpj').mask('99.999.999/9999-99');
        $('.zip-code').mask('99999-999');
        $('.phone').mask("(99) 9999-9999?9", {
            completed: function () {
                this.mask("(99) 99999-999?9");
            }
        });
        
        $('.date-time').blur(function () {
            var $this = $(this);
            if ($this.val().length == 16) {
                $this.val($this.val() + ':00');
            }
        });

        $('.money').maskMoney({
            symbol: '',
            showSymbol: true,
            thousands: '.',
            decimal: ',',
            symbolStay: false,
            defaultZero: true
        });

        $('.money-negative').maskMoney({
            symbol: '',
            showSymbol: true,
            thousands: '.',
            decimal: ',',
            symbolStay: false,
            defaultZero: true,
            allowNegative: true
        });
        
        $('.close').click(function (e) {
            $('.thunder-notification').slideUp();
            e.preventDefault();
        });

        $("#main-nav li ul").hide();
        $("#main-nav li a.current").parent().find("ul").slideToggle("slow");

        $("#main-nav li a.nav-top-item").click(
			function () {
			    $(this).parent().siblings().find("ul").slideUp("normal");
			    $(this).next().slideToggle("normal");
			    return false;
			}
		);

        $("#main-nav li a.no-submenu").click(
			function () {
			    window.location.href = (this.href);
			    return false;
			}
		);

        $("#main-nav li .nav-top-item").hover(
			function () {
			    $(this).stop().animate({ paddingRight: "25px" }, 200);
			},
			function () {
			    $(this).stop().animate({ paddingRight: "15px" });
			}
		);

        $(".closed-box .content-box-content").hide();
        $(".closed-box .content-box-tabs").hide();
        $('.content-box .content-box-content div.tab-content').hide();
        $('ul.content-box-tabs li a.default-tab').addClass('current');
        $('.content-box-content div.default-tab').show();
    });
})(jQuery);
