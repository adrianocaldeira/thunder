(function ($) {
    var manager;
    $.manager = manager = {
        root: '',
        utility: {
            resetDropDown: function (selector) {
                var $dropDown = $(selector);
                $('option', $dropDown).removeAttr('selected');
                $('option:first', $dropDown).attr('selected', 'selected');
            },
            resetText: function (selector) {
                $(selector).val('');
            },
            submitButton: function (button, form, options) {
                var settings = $.extend({}, {
                    before: null
                }, options);
                var $button = $(button);
                var $form = $(form);

                $form.data('button', $button);

                $button.click(function () {
                    if (settings.before) {
                        settings.before.call($form, $button);
                    } else {
                        $form.submit();
                    }
                });

                $('input[type="text"]', $form).keypress(function (e) {
                    if (e.keyCode == 13) {
                        $button.trigger('click');
                        e.preventDefault();
                    }
                });
            },
            setReadOnly: function (elements) {
                $.each(elements, function () {
                    var $element = $(this);
                    if ($element.is('a') || $element.is('input:button')) {
                        $element.remove();
                    } else {
                        if ($element.is('select')) {
                            $element.attr('disabled', 'disabled');
                        } else {
                            $element.attr('readonly', 'readonly');
                            if ($element.is('.date')) {
                                $element.unmask();
                                $element.datepicker('destroy');
                            }
                        }
                        $element.addClass('readonly');
                    }
                });
            },
            delete: function (selector, options) {
                var settings = $.extend({
                    success: function () {
                    }
                }, options);

                $(selector).live('click', function (e) {
                    var $this = $(this);

                    if ($this.data('destroy')) {
                        $.confirm($this.data('message'), {
                            onYes: function () {
                                $.ajax({
                                    url: $this.attr('href'),
                                    type: 'delete',
                                    success: function (r) {
                                        settings.success.call($this, r);
                                    }
                                });
                            }
                        });
                    } else {
                        $.alert('Registro não pode ser excluído, pois encontra-se relacionado com outra funcionalidade do sistema.', { height: 140 });
                    }

                    e.preventDefault();
                });
            }
        },
        login: {
            index: function () {
                $('form').ajaxForm({
                    message: $('#message'),
                    onSuccess: function (f, r) {
                        window.location.href = r.Data.Path;
                    }
                });
            }
        },
        users: {
            index: function () {
                $('#grid-users').grid();
            },
            form: function () {
                var $form = $('form');
                
                $form.ajaxForm({
                    loading: $('#loading'),
                    onBeforeSubmit: function () {
                        $form.find('.functionality').remove();
                        $.each($('.permission:input:checkbox:checked'), function (i) {
                            $form.append('<input type="hidden" class="functionality" name="Functionalities[' + i + '].Id" value="' + $(this).val() + '" />');
                        });
                    },
                    onSuccess: function (f, r) {
                        window.location.href = r.Data;
                    }
                });

                $('a.permission').click(function (e) {
                    var $this = $(this);
                    var $dialog = $('#permission-modal-' + $this.data('module'));

                    $dialog.dialog({
                        modal: true,
                        title: 'Permissões'
                    });
 
                    e.preventDefault();
                });

                $('.tree-view input:checkbox:not(.permission)').click(function () {
                    var $checkbox = $(this);
                    var $li = $checkbox.parents('li:first');
                    var $childs = $('input:checkbox', $li);
                    var $parents = $('input:checkbox:first', $li.parents('li'));
                    
                    if ($checkbox.is(':checked')) {
                        $parents.attr('checked', 'checked');
                        $childs.attr('checked', 'checked');
                        $.each($childs, function () {
                            var $this = $(this);
                            var $parentChild = $this.parents('li:first');
                            var $dialog = $('#permission-modal-' + $parentChild.data('module'));
                            $('input:checkbox', $dialog).attr('checked', 'checked');
                        });
                    } else {
                        $childs.removeAttr('checked');
                        $('input:checkbox', $('#permission-modal-' + $li.data('module'))).removeAttr('checked');
                        $.each($childs, function () {
                            var $this = $(this);
                            var $parentChild = $this.parents('li:first');
                            var $dialog = $('#permission-modal-' + $parentChild.data('module'));
                            $('input:checkbox', $dialog).removeAttr('checked');
                        });
                        
                    }
                });
                
                $('.tree-view input.permission:checkbox').click(function () {
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
            buttonImage: manager.root + 'content/manager/icons/calendar.png',
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
        $('.time').mask('99:99');
        $('.cpf').mask('999.999.999-99');
        $('.cnpj').mask('99.999.999/9999-99');
        $('.zip-code').mask('99999-999');
        $('.phone').mask("(99) 9999-9999?9", {
            completed: function () {
                this.mask("(99) 99999-999?9");
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
