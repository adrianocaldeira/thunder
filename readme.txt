################################################################
Thunder
################################################################

Configuração
================================================================
Após a instalação é necessário copiar as linhas abaixo e colar dentro do nó do xml ontem sua sessionfactory está configurada.

<session-factory>
	...
	<listener type="pre-update" class="Thunder.Data.Pattern.CreatedAndUpdatedPropertyEventListener, Thunder"/>
	<listener type="pre-insert" class="Thunder.Data.Pattern.CreatedAndUpdatedPropertyEventListener, Thunder"/>
</session-factory>

