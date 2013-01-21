(function ($) {
    var site;
    $.site = site = {
        root: '',
        utility: {
            resetDropDown: function(selector) {
                var $dropDown = $(selector);
                $('option', $dropDown).removeAttr('selected');
                $('option:first', $dropDown).attr('selected', 'selected');
            },
            resetText: function(selector) {
                $(selector).val('');
            },
            submitButton: function(button, form, options) {
                var settings = $.extend({ }, {
                    before: null
                }, options);
                var $button = $(button);
                var $form = $(form);

                $form.data('button', $button);

                $button.click(function() {
                    if (settings.before) {
                        settings.before.call($form, $button);
                    } else {
                        $form.submit();
                    }
                });

                $('input[type="text"]', $form).keypress(function(e) {
                    if (e.keyCode == 13) {
                        $button.trigger('click');
                        e.preventDefault();
                    }
                });
            },
            setReadOnly: function(elements) {
                $.each(elements, function() {
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
            delete: function(selector, options) {
                var settings = $.extend({
                    success: function() {
                    }
                }, options);

                $(selector).live('click', function(e) {
                    var $this = $(this);

                    if ($this.data('delete')) {
                        $.confirm($this.data('message'), {
                            onYes: function() {
                                $.ajax({
                                    url: $this.attr('href'),
                                    type: 'delete',
                                    success: function(r) {
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
        }
    };
    
    $(function () {
        site.root = $('body').data('root');

        $.thunder.settings.images.loadingModal = site.root + 'content/jquerythunder/images/loading_modal.gif';
        $.thunder.settings.images.loadingGrid = site.root + 'content/jquerythunder/images/loading_grid.gif';
        $.thunder.settings.images.loadingForm = site.root + 'content/jquerythunder/images/loading_form.gif';
        $.thunder.settings.confirm.label.yes = 'Sim';
        $.thunder.settings.confirm.label.no = 'Não';

        $.datepicker.setDefaults($.datepicker.regional['pt-BR']);

        $('.date').datepicker({
            showOn: 'button',
            buttonImage: site.root + 'content/site/icons/calendar.png',
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
    });
})(jQuery);
